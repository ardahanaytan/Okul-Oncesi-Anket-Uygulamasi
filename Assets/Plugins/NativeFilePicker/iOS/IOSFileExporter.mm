#import <UIKit/UIKit.h>

extern "C" void _ios_export_to_files(const char* cPath, const char* cUti, const char* cSuggestedName)
{
    @autoreleasepool
    {
        NSString* path = [NSString stringWithUTF8String:cPath];
        NSURL* url = [NSURL fileURLWithPath:path];

        // iOS “Save to Files” için Export modunda Document Picker
        UIDocumentPickerViewController* picker =
            [[UIDocumentPickerViewController alloc] initWithURLs:@[url]
                                                          inMode:UIDocumentPickerModeExportToService];

        picker.modalPresentationStyle = UIModalPresentationFormSheet;

        // En üstteki VC’yi bul ve göster
        UIWindow *keyWindow = [UIApplication sharedApplication].keyWindow;
        if (!keyWindow && @available(iOS 13.0, *)) {
            for (UIWindow* window in [UIApplication sharedApplication].windows) {
                if (window.isKeyWindow) { keyWindow = window; break; }
            }
            if (!keyWindow) keyWindow = [UIApplication sharedApplication].windows.firstObject;
        }
        UIViewController* root = keyWindow.rootViewController;
        while (root.presentedViewController) { root = root.presentedViewController; }

        [root presentViewController:picker animated:YES completion:nil];
    }
}
