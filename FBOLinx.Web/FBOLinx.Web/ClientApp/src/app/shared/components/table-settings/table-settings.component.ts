import { Component, Inject } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import {
    MatDialogRef,
    MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { SortDirection } from '@angular/material/sort';

export type ColumnType = {
    id: string;
    name: string;
    sort?: SortDirection;
}

@Component({
    selector: 'app-table-settings',
    templateUrl: './table-settings.component.html',
    styleUrls: ['./table-settings.component.scss'],
})
export class TableSettingsComponent {
    visibleColumns: ColumnType[] = [];
    invisibleColumns: ColumnType[] = [];

    constructor(
        private dialogRef: MatDialogRef<TableSettingsComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { visibleColumns: ColumnType[], invisibleColumns: ColumnType[] }
    ) {
        this.visibleColumns = [...data.visibleColumns];
        this.invisibleColumns = [...data.invisibleColumns];
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
        this.invisibleColumns = this.invisibleColumns.map(column => ({ id: column.id, name: column.name }));
        if (!this.visibleColumns.find(column => column.sort) && this.visibleColumns.length) {
            this.visibleColumns[0] = {
                ...this.visibleColumns[0],
                sort: 'asc',
            };
        }
    }

    onSortChange(column: ColumnType) {
        if (!column.sort) {
            column.sort = 'asc';
            this.visibleColumns = this.visibleColumns.map(vc =>
                vc.id === column.id ? column : {
                    id: vc.id,
                    name: vc.name,
                }
            );
        } else if (column.sort === 'asc') {
            column.sort = 'desc';
        } else if (column.sort === 'desc') {
            column.sort = 'asc';
        }
    }

    onSave() {
        this.dialogRef.close({
            visibleColumns: this.visibleColumns,
            invisibleColumns: this.invisibleColumns,
        });
    }

    onCancel() {
        this.dialogRef.close();
    }
}
