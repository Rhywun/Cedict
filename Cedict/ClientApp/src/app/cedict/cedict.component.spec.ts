import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CedictComponent } from './cedict.component';

describe('CedictComponent', () => {
  let component: CedictComponent;
  let fixture: ComponentFixture<CedictComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CedictComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CedictComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
