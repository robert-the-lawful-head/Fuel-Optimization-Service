import { OnInit, Component, Inject, ViewChild } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { PricingtemplatesService } from "../../../../services/pricingtemplates.service";
import { RichTextEditorComponent } from "@syncfusion/ej2-angular-richtexteditor";

@Component({
    selector: 'app-distribution-wizard-review',
    templateUrl: './distribution-wizard-review.component.html',
    styleUrls: ['./distribution-wizard-review.component.scss']
})
/** distribution-wizard-rewiew component*/
export class DistributionWizardReviewComponent implements OnInit {
    public edit: boolean = false;
    @ViewChild('typeEmail') rteEmail: RichTextEditorComponent;

    constructor(public dialogRef: MatDialogRef<DistributionWizardReviewComponent>, @Inject(MAT_DIALOG_DATA) public data: any,
        private pricingTemplatesService: PricingtemplatesService) { }

    ngOnInit() { }

    public changeStatus() {
        this.edit = true;
    }

    public update() {
        this.pricingTemplatesService.update(this.data).subscribe((data: any) => {

            this.edit = false;
        });
    }

    public disableToolbarEmail() {
        this.rteEmail.toolbarSettings.enable = false;

    }

    public enableToolbarEmail() {
        this.rteEmail.toolbarSettings.enable = true;
    }
}
