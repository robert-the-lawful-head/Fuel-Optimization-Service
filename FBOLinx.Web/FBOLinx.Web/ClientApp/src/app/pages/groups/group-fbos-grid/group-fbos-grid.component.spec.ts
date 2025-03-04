/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { GroupFbosGridComponent } from './group-fbos-grid.component';

describe('GroupFbosGridComponent', () => {
    let component: GroupFbosGridComponent;
    let fixture: ComponentFixture<GroupFbosGridComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [GroupFbosGridComponent],
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(GroupFbosGridComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
