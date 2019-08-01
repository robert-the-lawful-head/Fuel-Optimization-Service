/// <reference path="../../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { NeedsAttentionComponent } from './needs-attention.component';

let component: NeedsAttentionComponent;
let fixture: ComponentFixture<NeedsAttentionComponent>;

describe('needs-attention component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ NeedsAttentionComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(NeedsAttentionComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});