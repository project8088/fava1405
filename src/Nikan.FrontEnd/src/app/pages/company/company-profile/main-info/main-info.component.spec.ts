import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyMainInfoComponent } from './main-info.component';

describe('CompanyMainInfoComponent', () => {
  let component: CompanyMainInfoComponent;
  let fixture: ComponentFixture<CompanyMainInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompanyMainInfoComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyMainInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
