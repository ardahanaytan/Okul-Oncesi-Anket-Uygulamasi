#import <UIKit/UIKit.h>
#import <MessageUI/MessageUI.h>
#import <objc/runtime.h>

extern "C" UIViewController* UnityGetGLViewController(void);

@interface MailComposerHelper : NSObject <MFMailComposeViewControllerDelegate>
@end

@implementation MailComposerHelper
- (void)mailComposeController:(MFMailComposeViewController *)controller
          didFinishWithResult:(MFMailComposeResult)result
                        error:(NSError *)error
{
    [controller dismissViewControllerAnimated:YES completion:nil];
}
@end

// Helper'ı güçlü referansla tut (yoksa deallocate olabilir)
static MailComposerHelper *g_mailHelper = nil;

extern "C" void _ComposeMail(const char* toCsv,
                             const char* subject,
                             const char* body,
                             const char* filePath,
                             const char* attachmentName)
{
    dispatch_async(dispatch_get_main_queue(), ^{
        UIViewController *root = UnityGetGLViewController();

        if (![MFMailComposeViewController canSendMail]) {
            // Kullanıcıya bilgi ver
            UIAlertController* alert =
              [UIAlertController alertControllerWithTitle:@"Mail gönderilemiyor"
                                                  message:@"Bu cihazda Mail hesabı yapılandırılmamış. Ayarlar > Mail üzerinden hesap ekleyin."
                                           preferredStyle:UIAlertControllerStyleAlert];
            [alert addAction:[UIAlertAction actionWithTitle:@"Tamam"
                                                      style:UIAlertActionStyleDefault
                                                    handler:nil]];
            [root presentViewController:alert animated:YES completion:nil];
            return;
        }

        if (!g_mailHelper) g_mailHelper = [MailComposerHelper new];

        NSString *toStr = [NSString stringWithUTF8String:toCsv ?: ""];
        NSArray<NSString*> *recipients =
            toStr.length ? [toStr componentsSeparatedByString:@","] : @[];

        MFMailComposeViewController *vc = [MFMailComposeViewController new];
        vc.mailComposeDelegate = g_mailHelper;

        [vc setToRecipients:recipients];
        [vc setSubject:[NSString stringWithUTF8String:subject ?: ""]];
        [vc setMessageBody:[NSString stringWithUTF8String:body ?: ""] isHTML:NO];

        if (filePath && attachmentName) {
            NSString *path = [NSString stringWithUTF8String:filePath];
            NSData *data = [NSData dataWithContentsOfFile:path];
            if (data) {
                NSString *name = [NSString stringWithUTF8String:attachmentName];
                [vc addAttachmentData:data mimeType:@"text/csv" fileName:name];
            }
        }

        [root presentViewController:vc animated:YES completion:nil];
    });
}
