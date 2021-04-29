import { Component, Inject } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { MAT_DIALOG_DATA, MatDialogRef, } from '@angular/material/dialog';
import { SortDirection } from '@angular/material/sort';

export type ColumnType = {
    id: string;
    name: string;
    sort?: SortDirection;
    hidden?: boolean;
};

@Component({
    selector: 'app-table-settings',
    templateUrl: './table-settings.component.html',
    styleUrls: [ './table-settings.component.scss' ],
})
export class TableSettingsComponent {
    columns: ColumnType[] = [];

    constructor(
        private dialogRef: MatDialogRef<TableSettingsComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ColumnType[]
    ) {
        this.columns = [...data];
    }

    drop(event: CdkDragDrop<string[]>) {
        if (event.previousContainer === event.container) {
            moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
        } else {
            transferArrayItem(
                event.previousContainer.data,
                event.container.data,
                event.previousIndex,
                event.currentIndex
            );
        }
    }

    onSortChange(column: ColumnType) {
        if (!column.sort) {
            column.sort = 'asc';
            this.columns = this.columns.map(vc =>
                vc.id === column.id ? column : {
                    id: vc.id,
                    name: vc.name,
                    hidden: vc.hidden,
                }
            );
        } else if (column.sort === 'asc') {
            column.sort = 'desc';
        } else if (column.sort === 'desc') {
            column.sort = 'asc';
        }
    }

    toggleColumnVisible(column: ColumnType) {
        this.columns = this.columns.map(c => c.id === column.id ? ({
            ...column,
            hidden: !column.hidden
        }): c);
    }

    onSave() {
        this.dialogRef.close(this.columns);
    }

    onCancel() {
        this.dialogRef.close();
    }
}
