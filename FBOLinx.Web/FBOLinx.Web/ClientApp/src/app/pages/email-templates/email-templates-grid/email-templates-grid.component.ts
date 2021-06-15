import { Component, EventEmitter, Input, OnInit, Output, ViewChild, } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

// Services
import { SharedService } from '../../../layouts/shared-service';

import { CopyConfirmationComponent } from '../../../shared/components/copy-confirmation/copy-confirmation.component';
import { EmailTemplatesDialogNewTemplateComponent } from '../../../shared/components/email-templates-dialog-new-template/email-templates-dialog-new-template.component';

@Component({
    selector: 'app-email-templates-grid',
    templateUrl: './email-templates-grid.component.html',
    styleUrls: ['./email-templates-grid.component.scss'],
})
export class EmailTemplatesGridComponent implements OnInit {
    @Output() editEmailTemplateClicked = new EventEmitter<any>();
    @Output() deleteEmailTemplateClicked = new EventEmitter<any>();
    @Output() copyEmailTemplateClicked = new EventEmitter<any>();
    @Output() newEmailTemplateAdded = new EventEmitter<any>();
    @Input() emailTemplatesData: any[];
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    public emailTemplatesDataSource: MatTableDataSource<any> = null;
    public displayedColumns: string[] = [
        'name',
        'subject',
        'copy',
        'delete'
    ];

    public pageIndexTemplate = 0;
    public pageSizeTemplate = 50;

    constructor(public newTemplateDialog: MatDialog,
        public copyTemplateDialog: MatDialog,
        public deleteTemplateWarningDialog: MatDialog,
        private sharedService: SharedService) {

    }

    ngOnInit(): void {
        if (!this.emailTemplatesData) {
            return;
        }

        this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));
        this.emailTemplatesDataSource = new MatTableDataSource(
            this.emailTemplatesData
        );
        this.emailTemplatesDataSource.sort = this.sort;
        this.emailTemplatesDataSource.paginator = this.paginator;

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

    public editEmailTemplate(emailTemplate) {
        this.editEmailTemplateClicked.emit({
            emailTemplateId: emailTemplate.oid,
            filter: this.emailTemplatesDataSource.filter,
            page: this.emailTemplatesDataSource.paginator.pageIndex,
            order: this.emailTemplatesDataSource.sort.active,
            orderBy: this.emailTemplatesDataSource.sort.direction,
        });
    }

    public addNewEmailTemplate() {
        const dialogRef = this.newTemplateDialog.open(EmailTemplatesDialogNewTemplateComponent, {
            data: {
                fboId: this.sharedService.currentUser.fboId,
            },
        });

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

                this.copyEmailTemplateClicked.emit(result);;
            });
        }
    }

    public applyFilter(filterValue: string) {
        this.emailTemplatesDataSource.filter = filterValue
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
