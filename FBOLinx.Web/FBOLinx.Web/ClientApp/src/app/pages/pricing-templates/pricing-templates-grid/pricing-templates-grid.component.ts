import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource, MatTable } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { PricetiersService } from '../../../services/pricetiers.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { PricingTemplatesDialogNewTemplateComponent } from '../pricing-templates-dialog-new-template/pricing-templates-dialog-new-template.component';
import { Router } from '@angular/router';

@Component({
    selector: 'app-pricing-templates-grid',
    templateUrl: './pricing-templates-grid.component.html',
    styleUrls: ['./pricing-templates-grid.component.scss']
})
/** pricing-templates-grid component*/
export class PricingTemplatesGridComponent implements OnInit {

    //Input/Output Bindings
    @Output() editPricingTemplateClicked = new EventEmitter<any>();
    @Output() deletePricingTemplateClicked = new EventEmitter<any>();
    @Output() newPricingTemplateAdded = new EventEmitter<any>();
    @Input() pricingTemplatesData: Array<any>;

    //Public Members
    public pricingTemplatesDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['isInvalid', 'name', 'marginTypeDescription', 'yourMargin', 'intoPlanePrice', 'notes', 'delete'];
    public resultsLength: number = 0;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    /** pricing-templates-grid ctor */
    constructor(public newTemplateDialog: MatDialog, private router: Router,
        private pricingTemplatesService: PricingtemplatesService,
        private priceTiersService: PricetiersService,
        private sharedService: SharedService    ) {

    }

    ngOnInit() {
        if (!this.pricingTemplatesData)
            return;
        this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
        this.pricingTemplatesDataSource = new MatTableDataSource(this.pricingTemplatesData);
        this.pricingTemplatesDataSource.sort = this.sort;
        this.pricingTemplatesDataSource.paginator = this.paginator;
        this.resultsLength = this.pricingTemplatesData.length;
    }

    public editPricingTemplate(pricingTemplate, $event) {
        if ($event.srcElement.nodeName.toLowerCase() == 'button' || $event.srcElement.nodeName.toLowerCase() == 'select' || ($event.srcElement.nodeName.toLowerCase() == 'input' && $event.srcElement.getAttribute('type') == 'checkbox')) {
            //$event.preventDefault();
            $event.stopPropagation();
            return;
        }
        const clonedRecord = Object.assign({}, pricingTemplate);
        this.editPricingTemplateClicked.emit(clonedRecord);
    }

    public addNewPricingTemplate() {
        const dialogRef = this.newTemplateDialog.open(PricingTemplatesDialogNewTemplateComponent,
            {
                data: {
                    fboId: this.sharedService.currentUser.fboId,
                    marginType: 1,
                    customerMargins: [{min: 1, max: 99999, amount: 0, itp: 0}]
                }
            });

        dialogRef.afterClosed().subscribe(result => {
            if (!result)
                return;
            this.newPricingTemplateAdded.emit();
        });
    }

    public deletePricingTemplate(pricingTemplate) {
        this.deletePricingTemplateClicked.emit(pricingTemplate);
    }

    public applyFilter(filterValue: string) {
        this.pricingTemplatesDataSource.filter = filterValue.trim().toLowerCase();
    }
}
