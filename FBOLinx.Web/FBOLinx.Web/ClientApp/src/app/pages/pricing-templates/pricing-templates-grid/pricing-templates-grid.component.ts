import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
} from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource, MatTable } from "@angular/material/table";
import {
    MatDialog,
    MatDialogRef,
    MAT_DIALOG_DATA,
} from "@angular/material/dialog";

// Services
import { SharedService } from "../../../layouts/shared-service";
import { CustomcustomertypesService } from "../../../services/customcustomertypes.service";

// Components
import { PricingTemplatesDialogNewTemplateComponent } from "../pricing-templates-dialog-new-template/pricing-templates-dialog-new-template.component";
import { PricingTemplatesDialogCopyTemplateComponent } from "../pricing-template-dialog-copy-template/pricing-template-dialog-copy-template.component";
import { Router } from "@angular/router";
import { PricingTemplatesDialogDeleteWarningComponent } from "../pricing-template-dialog-delete-warning-template/pricing-template-dialog-delete-warning.component";

export interface DefaultTemplateUpdate {
    currenttemplate: number;
    newtemplate: number;
    fboid: number;
}

@Component({
    selector: "app-pricing-templates-grid",
    templateUrl: "./pricing-templates-grid.component.html",
    styleUrls: ["./pricing-templates-grid.component.scss"],
})
export class PricingTemplatesGridComponent implements OnInit {
    // Input/Output Bindings
    @Output() editPricingTemplateClicked = new EventEmitter<any>();
    @Output() deletePricingTemplateClicked = new EventEmitter<any>();
    @Output() copyPricingTemplateClicked = new EventEmitter<any>();
    @Output() newPricingTemplateAdded = new EventEmitter<any>();
    @Input() pricingTemplatesData: Array<any>;

    // Public Members
    public pricingTemplatesDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = [
        "isInvalid",
        "name",
        "marginTypeDescription",
        "yourMargin",
        "intoPlanePrice",
        "customersAssigned",
        "copy",
        "delete",
    ];
    public resultsLength = 0;

    public pageIndexTemplate = 0;
    public pageSizeTemplate = 50;

    public updateModel: DefaultTemplateUpdate = {
        currenttemplate: 0,
        fboid: 0,
        newtemplate: 0,
    };

    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor(
        public newTemplateDialog: MatDialog,
        public copyTemplateDialog: MatDialog,
        public deleteTemplateWarningDialog: MatDialog,
        private sharedService: SharedService,
        public customCustomerService: CustomcustomertypesService
    ) {}

    ngOnInit() {
        if (!this.pricingTemplatesData) {
            return;
        }

        if (localStorage.getItem("pageIndexTemplate")) {
            this.paginator.pageIndex = localStorage.getItem("pageIndexTemplate") as any;
        } else {
            this.paginator.pageIndex = 0;
        }

        if (sessionStorage.getItem("pageSizeValueTemplate")) {
            this.pageSizeTemplate = sessionStorage.getItem("pageSizeValueTemplate") as any;
        } else {
            this.pageSizeTemplate = 50;
        }

        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.pricingTemplatesDataSource = new MatTableDataSource(
            this.pricingTemplatesData
        );
        this.pricingTemplatesDataSource.sort = this.sort;
        this.pricingTemplatesDataSource.paginator = this.paginator;
        this.resultsLength = this.pricingTemplatesData.length;

        this.updateModel.currenttemplate = 0;
    }

    public editPricingTemplate(pricingTemplate, $event) {
        if (
            $event.srcElement.nodeName.toLowerCase() === "button" ||
            $event.srcElement.nodeName.toLowerCase() === "select" ||
            ($event.srcElement.nodeName.toLowerCase() === "input" &&
                $event.srcElement.getAttribute("type") === "checkbox")
        ) {
            $event.stopPropagation();
            return;
        }
        const clonedRecord = Object.assign({}, pricingTemplate);
        this.editPricingTemplateClicked.emit(clonedRecord);
    }

    public addNewPricingTemplate() {
        const dialogRef = this.newTemplateDialog.open(
            PricingTemplatesDialogNewTemplateComponent,
            {
                data: {
                    fboId: this.sharedService.currentUser.fboId,
                    marginType: 1,
                    customerMargins: [
                        { min: 1, max: 99999, amount: 0, itp: 0, allin: 0 },
                    ],
                },
            }
        );

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.newPricingTemplateAdded.emit();
            this.sharedService.NotifyPricingTemplateComponent(
                "updatecomponent"
            );
        });
    }

    public deletePricingTemplate(pricingTemplate) {
        if (pricingTemplate.default) {
            const dialogRef = this.deleteTemplateWarningDialog.open(
                PricingTemplatesDialogDeleteWarningComponent,
                {
                    data: this.pricingTemplatesData,
                }
            );

            dialogRef.afterClosed().subscribe((response) => {
                if (response !== "cancel") {
                    this.updateModel.currenttemplate = pricingTemplate.oid;
                    this.updateModel.fboid = pricingTemplate.fboid;
                    this.updateModel.newtemplate = response;

                    this.customCustomerService
                        .updateDefaultTemplate(this.updateModel)
                        .subscribe((result) => {
                            if (result) {
                                this.newPricingTemplateAdded.emit();
                            }
                        });
                }
            });
        } else {
            this.deletePricingTemplateClicked.emit(pricingTemplate);
        }
    }

    public copyPricingTemplate(pricingTemplate) {
        if (pricingTemplate) {
            const dialogRef = this.copyTemplateDialog.open(
                PricingTemplatesDialogCopyTemplateComponent,
                {
                    data: { currentPricingTemplateId: pricingTemplate.oid },
                }
            );

            dialogRef.afterClosed().subscribe((result) => {});
        }
    }

    public applyFilter(filterValue: string) {
        this.pricingTemplatesDataSource.filter = filterValue
            .trim()
            .toLowerCase();
    }

    onPageChanged(event: any) {
        localStorage.setItem("pageIndexTemplate", event.pageIndex);
        sessionStorage.setItem(
            "pageSizeValueTemplate",
            this.paginator.pageSize.toString()
        );
    }
}
