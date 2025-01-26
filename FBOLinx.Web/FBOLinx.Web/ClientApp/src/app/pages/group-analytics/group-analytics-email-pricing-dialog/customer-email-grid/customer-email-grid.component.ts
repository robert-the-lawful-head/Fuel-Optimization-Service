import { Component, Input, OnInit } from '@angular/core';
import { defaultStringsEnum } from 'src/app/enums/strings.enums';
import { NullOrEmptyToDefault } from 'src/app/shared/pipes/null/NullOrEmptyToDefault.pipe';

@Component({
    selector: 'app-customer-email-grid',
    templateUrl: './customer-email-grid.component.html',
    styleUrls: ['./customer-email-grid.component.scss'],
    providers: [NullOrEmptyToDefault],
})
export class CustomerEmailGridComponent implements OnInit {
    @Input() dataSource: any[];

    columns = [];
    displayedColumns: string[] = [];

    constructor(private nullOrEmptyToDefaultPipe: NullOrEmptyToDefault) {
        this.columns = [
            {
                columnDef: 'company',
                header: 'Company',
                cell: (element: any) =>
                    `${this.nullOrEmptyToDefaultPipe.transform(
                        element.company,
                        defaultStringsEnum.empty
                    )}`,
            },
            {
                columnDef: 'firstName',
                header: 'First Name',
                cell: (element: any) =>
                    `${this.nullOrEmptyToDefaultPipe.transform(
                        element.firstName,
                        defaultStringsEnum.empty
                    )}`,
            },
            {
                columnDef: 'lastName',
                header: 'Last Name',
                cell: (element: any) =>
                    `${this.nullOrEmptyToDefaultPipe.transform(
                        element.lastName,
                        defaultStringsEnum.empty
                    )}`,
            },
            {
                columnDef: 'email',
                header: 'Email',
                cell: (element: any) =>
                    `${this.nullOrEmptyToDefaultPipe.transform(
                        element.email,
                        defaultStringsEnum.empty
                    )}`,
            },
        ];
        this.displayedColumns = this.columns.map((c) => c.columnDef);
    }

    ngOnInit() {}
}
