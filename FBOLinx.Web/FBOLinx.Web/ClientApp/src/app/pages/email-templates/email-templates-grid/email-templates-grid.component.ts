import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { MatLegacyDialog as MatDialog } from '@angular/material/legacy-dialog';
import { MatLegacyPaginator as MatPaginator } from '@angular/material/legacy-paginator';
import { MatSort } from '@angular/material/sort';
import { MatLegacyTableDataSource as MatTableDataSource } from '@angular/material/legacy-table';
import { GridBase } from 'src/app/services/tables/GridBase';

// Services
import { SharedService } from '../../../layouts/shared-service';
import { CopyConfirmationComponent } from '../../../shared/components/copy-confirmation/copy-confirmation.component';
import { EmailTemplatesDialogNewTemplateComponent } from '../../../shared/components/email-templates-dialog-new-template/email-templates-dialog-new-template.component';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-email-templates-grid',
    styleUrls: ['./email-templates-grid.component.scss'],
    templateUrl: './email-templates-grid.component.html',
})
export class EmailTemplatesGridComponent extends GridBase implements OnInit {
    @Output() editEmailTemplateClicked = new EventEmitter<any>();
    @Output() deleteEmailTemplateClicked = new EventEmitter<any>();
    @Output() copyEmailTemplateClicked = new EventEmitter<any>();
    @Output() newEmailTemplateAdded = new EventEmitter<any>();
    @Input() emailTemplatesData: any[];
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    public dataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = ['name', 'subject', 'copy', 'delete'];

    sortChangeSubscription: Subscription;
    constructor(
        public newTemplateDialog: MatDialog,
        public copyTemplateDialog: MatDialog,
        public deleteTemplateWarningDialog: MatDialog,
        private sharedService: SharedService
    ) {
        super();
    }

    ngOnInit(): void {
        if (!this.emailTemplatesData) {
            return;
        }

        this.sortChangeSubscription = this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.dataSource = new MatTableDataSource(
            this.emailTemplatesData
        );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;

        //this.updateModel.currenttemplate = 0;

        //this.store.select(getPricingTemplateState).subscribe(state => {
        //    if (state.filter) {
        //        this.pricingTemplatesDataSource.filter = state.filter;
        //    }
        //    if (state.page) {
        //        this.paginator.pageIndex = state.page;
        //    }
        //    if (state.order) {
        //        this.sort.active = state.order;
        //    }
        //    if (state.orderBy) {
        //        this.sort.direction = state.orderBy as SortDirection;
        //    }
        //});
    }
    ngOnDestroy() {
        this.sortChangeSubscription?.unsubscribe();
    }
    public editEmailTemplate(emailTemplate) {
        this.editEmailTemplateClicked.emit({
            emailTemplateId: emailTemplate.oid,
            filter: this.dataSource.filter,
            order: this.dataSource.sort.active,
            orderBy: this.dataSource.sort.direction,
            page: this.dataSource.paginator.pageIndex,
        });
    }

    public addNewEmailTemplate() {
        const dialogRef = this.newTemplateDialog.open(
            EmailTemplatesDialogNewTemplateComponent,
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
            this.newEmailTemplateAdded.emit(result);
        });
    }

    public deleteEmailTemplate(emailTemplate) {
        this.deleteEmailTemplateClicked.emit(emailTemplate);
    }

    public copyEmailTemplate(emailTemplate) {
        if (emailTemplate) {
            const clone: any = JSON.parse(JSON.stringify(emailTemplate));
            clone.oid = 0;
            clone.name = '';
            const dialogRef = this.copyTemplateDialog.open(
                CopyConfirmationComponent,
                {
                    data: clone,
                }
            );

            dialogRef.afterClosed().subscribe((result) => {
                if (!result) {
                    return;
                }

                this.copyEmailTemplateClicked.emit(result);
            });
        }
    }

    public applyFilter(filterValue: string) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    onPageChanged(event: any) {
        localStorage.setItem('pageIndexTemplate', event.pageIndex);
        sessionStorage.setItem(
            'pageSizeValueTemplate',
            this.paginator.pageSize.toString()
        );
    }
}
