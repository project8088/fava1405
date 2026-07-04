import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewJobDetailsDialogComponent } from './job-details.component';

describe('ViewJobDetailsDialogComponent', () => {
  let component: ViewJobDetailsDialogComponent;
  let fixture: ComponentFixture<ViewJobDetailsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ViewJobDetailsDialogComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewJobDetailsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
