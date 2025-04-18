; Wait for the file upload dialog to appear
WinWaitActive("Open")
 
; Set the file path in the dialog
ControlSetText("Open", "", "Edit1", "C:\Downloads\testfile")
 
; Click the 'Open' button
ControlClick("Open", "", "Button1")