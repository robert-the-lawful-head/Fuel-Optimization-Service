import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DocumentService } from 'src/app/services/documents.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-Agreements-and-documents-modal',
  templateUrl: './Agreements-and-documents-modal.component.html',
  styleUrls: ['./Agreements-and-documents-modal.component.scss']
})
export class AgreementsAndDocumentsModalComponent implements OnInit {
    constructor(
        public dialogRef: MatDialogRef<AgreementsAndDocumentsModalComponent>,
        private router: Router,
        private authenticationService: AuthenticationService,
        private documentService: DocumentService,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {    }

    ngOnInit() {
    }
    onCancelClick(): void {
        this.dialogRef.close();
    }
    accept() {
        this.documentService.acceptDocument(this.data.userId,this.data.eulaDocument.oid).subscribe(
            (data: any) => {
                this.dialogRef.close(true);
            },
            (err: any) => {
                console.log(err);
                this.dialogRef.close(false);
            }
        );
    }
    decline(){
        this.dialogRef.close(false);
        this.authenticationService.logout();
        this.router.navigate(['/landing-site-layout']);
    }
}
