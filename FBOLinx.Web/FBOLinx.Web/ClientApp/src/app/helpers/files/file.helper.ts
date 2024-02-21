import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class FileHelper{
    private imgSizeLimitinBytes: number = 1048576;  // 1 MB in bytes
    private imgLargeImageErrorMsg: string = "The image size exceeds the 1 MB limit. Please use an image smaller than 1 MB.";

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
