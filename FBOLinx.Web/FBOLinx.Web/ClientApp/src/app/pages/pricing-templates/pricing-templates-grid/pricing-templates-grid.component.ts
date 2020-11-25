import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import {MatSort, SortDirection} from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import {Store} from '@ngrx/store';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';

// Components
import { PricingTemplatesDialogNewTemplateComponent } from '../pricing-templates-dialog-new-template/pricing-templates-dialog-new-template.component';
import { PricingTemplatesDialogCopyTemplateComponent } from '../pricing-template-dialog-copy-template/pricing-template-dialog-copy-template.component';
import { PricingTemplatesDialogDeleteWarningComponent } from '../pricing-template-dialog-delete-warning-template/pricing-template-dialog-delete-warning.component';

import { getPricingTemplateState } from '../../../store/selectors/pricing-template';
import { State } from '../../../store/reducers';

export interface DefaultTemplateUpdate {
    currenttemplate: number;
    newtemplate: number;
    fboid: number;
}

@Component({
    selector: 'app-pricing-templates-grid',
    templateUrl: './pricing-templates-grid.component.html',
    styleUrls: ['./pricing-templates-grid.component.scss'],
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
        'isInvalid',
        'name',
        'marginTypeDescription',
        'customersAssigned',
        'copy',
        'delete',
    ];

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
        private store: Store<State>,
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

        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.pricingTemplatesDataSource = new MatTableDataSource(
            this.pricingTemplatesData
        );
        this.pricingTemplatesDataSource.sort = this.sort;
        this.pricingTemplatesDataSource.paginator = this.paginator;

        this.updateModel.currenttemplate = 0;

        this.store.select(getPricingTemplateState).subscribe(state => {
            if (state.filter) {
                this.pricingTemplatesDataSource.filter = state.filter;
            }
            if (state.page) {
                this.paginator.pageIndex = state.page;
            }
            if (state.order) {
                this.sort.active = state.order;
            }
            if (state.orderBy) {
                this.sort.direction = state.orderBy as SortDirection;
            }
        });
    }

    public editPricingTemplate(pricingTemplate) {
        this.editPricingTemplateClicked.emit({
            pricingTemplateId: pricingTemplate.oid,
            filter: this.pricingTemplatesDataSource.filter,
            page: this.pricingTemplatesDataSource.paginator.pageIndex,
            order: this.pricingTemplatesDataSource.sort.active,
            orderBy: this.pricingTemplatesDataSource.sort.direction,
        });
    }

    public addNewPricingTemplate() {
        const dialogRef = this.newTemplateDialog.open(PricingTemplatesDialogNewTemplateComponent, {
            data: {
                fboId: this.sharedService.currentUser.fboId,
            },
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (!result) {
                return;
            }
            this.newPricingTemplateAdded.emit();
            this.sharedService.NotifyPricingTemplateComponent(
                'updateComponent'
            );
        });
    }

    public deletePricingTemplate(pricingTemplate) {
        if (pricingTemplate.default) {
            const dialogRef = this.deleteTemplateWarningDialog.open(
                PricingTemplatesDialogDeleteWarningComponent,
                {
                    data: this.pricingTemplatesData,
                    autoFocus: false,
                    width: '600px',
                }
            );

            dialogRef.afterClosed().subscribe((response) => {
                if (response !== 'cancel') {
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
        localStorage.setItem('pageIndexTemplate', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeValueTemplate',
            this.paginator.pageSize.toString()
        );
    }
}
