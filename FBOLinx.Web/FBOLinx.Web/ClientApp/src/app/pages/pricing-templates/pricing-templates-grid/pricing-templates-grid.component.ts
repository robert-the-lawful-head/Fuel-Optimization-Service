import { Component, EventEmitter, Input, Output, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

//Services
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { SharedService } from '../../../layouts/shared-service';

//Components
import { PricingTemplatesDialogNewTemplateComponent } from '../pricing-templates-dialog-new-template/pricing-templates-dialog-new-template.component';

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
    @Input() pricingTemplatesData: Array<any>;

    //Public Members
    public pricingTemplatesDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['name', 'intoPlanePrice', 'notes', 'marginTypeDescription', 'default', 'edit', 'delete'];
    public resultsLength: number = 0;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    /** pricing-templates-grid ctor */
    constructor(public newTemplateDialog: MatDialog,
        private pricingTemplatesService: PricingtemplatesService,
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

    public editPricingTemplate(pricingTemplate) {
        const clonedRecord = Object.assign({}, pricingTemplate);
        this.editPricingTemplateClicked.emit(clonedRecord);
    }

    public addNewPricingTemplate() {
        const dialogRef = this.newTemplateDialog.open(PricingTemplatesDialogNewTemplateComponent, {
                data: {}
            });

        dialogRef.afterClosed().subscribe(result => {
            result.fboId = this.sharedService.currentUser.fboId;
            this.pricingTemplatesService.add(result).subscribe((data: any) => {
                this.editPricingTemplate(data);
            });
        });
    }

    public deletePricingTemplate(pricingTemplate) {
        this.deletePricingTemplateClicked.emit(pricingTemplate);
    }

    public applyFilter(filterValue: string) {
        this.pricingTemplatesDataSource.filter = filterValue.trim().toLowerCase();
    }
}
