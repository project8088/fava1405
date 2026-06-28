import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyBaseInfoComponent } from './base-info.component';

describe('CompanyBaseInfoComponent', () => {
  let component: CompanyBaseInfoComponent;
  let fixture: ComponentFixture<CompanyBaseInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyBaseInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyBaseInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
