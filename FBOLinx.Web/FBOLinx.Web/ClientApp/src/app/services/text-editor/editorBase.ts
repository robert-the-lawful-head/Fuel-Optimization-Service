import { FileInfo, SelectedEventArgs } from "@syncfusion/ej2-angular-inputs";
import { ImageDropEventArgs } from "@syncfusion/ej2-angular-richtexteditor";
import { FileHelper } from "src/app/helpers/files/file.helper";

export abstract class EditorBase {
    constructor(protected fileHelper: FileHelper){
    }
    public onImageSelected = (args: SelectedEventArgs) => {
        let lastImage : FileInfo = args.filesData[args.filesData.length - 1];
        if (!this.fileHelper.isValidImageSize(lastImage.size)) {
            args.cancel = true;
        }
    }
    onImageDrop(event : ImageDropEventArgs){
        //simulate disable drag and drop
        event.cancel = true;
    }
}
