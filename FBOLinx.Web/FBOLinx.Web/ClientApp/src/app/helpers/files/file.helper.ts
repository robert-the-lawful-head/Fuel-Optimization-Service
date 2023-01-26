import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class FileHelper{
    private imgSizeLimitinBytes: number = 60000;
    private imgLargeImageErrorMsg: string = "This image file size is too large, please use a smaller image.";

    constructor(private snackBar: MatSnackBar) { }

    isValidImageSize(imageSizeinBytes: number): boolean{
        if (this.imgSizeLimitinBytes < imageSizeinBytes) {
            this.snackBar.open(this.imgLargeImageErrorMsg, "X");
            return false;
        }
        else
            return true;
    }
}
