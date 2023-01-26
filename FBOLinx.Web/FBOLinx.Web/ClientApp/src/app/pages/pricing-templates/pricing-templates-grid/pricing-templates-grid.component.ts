import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, SortDirection } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Store } from '@ngrx/store';
import { VirtualScrollBase } from 'src/app/services/tables/VirtualScrollBase';

// Services
import { SharedService } from '../../../layouts/shared-service';
import * as SharedEvents from '../../../models/sharedEvents';
import { CustomcustomertypesService } from '../../../services/customcustomertypes.service';
import { PricingtemplatesService } from '../../../services/pricingtemplates.service';
import { State } from '../../../store/reducers';
import { getPricingTemplateState } from '../../../store/selectors/pricing-template';
import { PricingTemplatesDialogCopyTemplateComponent } from '../pricing-template-dialog-copy-template/pricing-template-dialog-copy-template.component';
import { PricingTemplatesDialogDeleteWarningComponent } from '../pricing-template-dialog-delete-warning-template/pricing-template-dialog-delete-warning.component';
// Components
import { PricingTemplatesDialogNewTemplateComponent } from '../pricing-templates-dialog-new-template/pricing-templates-dialog-new-template.component';

export interface DefaultTemplateUpdate {
    currenttemplate: number;
    newtemplate: number;
    fboid: number;
}

@Component({
    selector: 'app-pricing-templates-grid',
    styleUrls: ['./pricing-templates-grid.component.scss'],
    templateUrl: './pricing-templates-grid.component.html',
})
export class PricingTemplatesGridComponent extends VirtualScrollBase implements OnInit {
    // Input/Output Bindings
    @Output() editPricingTemplateClicked = new EventEmitter<any>();
    @Output() deletePricingTemplateClicked = new EventEmitter<any>();
    @Output() copyPricingTemplateClicked = new EventEmitter<any>();
    @Output() newPricingTemplateAdded = new EventEmitter<any>();
    @Input() pricingTemplatesData: Array<any>;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    // Public Members
    public dataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = [
        'isInvalid',
        'name',
        'marginTypeDescription',
        'pricingFormula',
        'allInPrice',
        'customersAssigned',
        'aircraftsAssigned',
        'copy',
        'delete',
    ];

    public updateModel: DefaultTemplateUpdate = {
        currenttemplate: 0,
        fboid: 0,
        newtemplate: 0,
    };

    constructor(
        private store: Store<State>,
        public newTemplateDialog: MatDialog,
        public copyTemplateDialog: MatDialog,
        public deleteTemplateWarningDialog: MatDialog,
        private sharedService: SharedService,
        public customCustomerService: CustomcustomertypesService,
        public pricingTemplatesService: PricingtemplatesService
    ) {
        super();
    }

    ngOnInit() {
        if (!this.pricingTemplatesData) {
            return;
        }

        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.dataSource = new MatTableDataSource(
            this.pricingTemplatesData
        );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;

        this.updateModel.currenttemplate = 0;

        this.store.select(getPricingTemplateState).subscribe((state) => {
            if (state.filter) {
                this.dataSource.filter = state.filter;
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
            filter: this.dataSource.filter,
            order: this.dataSource.sort.active,
            orderBy: this.dataSource.sort.direction,
            page: this.dataSource.paginator.pageIndex,
            pricingTemplateId: pricingTemplate.oid,
        });
    }

    public addNewPricingTemplate() {
        const dialogRef = this.newTemplateDialog.open(
            PricingTemplatesDialogNewTemplateComponent,
            {
                data: {
                    fboId: this.sharedService.currentUser.fboId,
                },
            }
        );

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
                    autoFocus: false,
                    data: this.pricingTemplatesData,
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

            dialogRef.afterClosed().subscribe((result) => {
                if (result != 'cancel') {
                    this.pricingTemplatesService.copy(result).subscribe((response) => {
                        this.newPricingTemplateAdded.emit();
                    });
                }
            });
        }
    }

    public applyFilter(filterValue: string) {
        this.dataSource.filter = filterValue
            .trim()
            .toLowerCase();
    }
}
