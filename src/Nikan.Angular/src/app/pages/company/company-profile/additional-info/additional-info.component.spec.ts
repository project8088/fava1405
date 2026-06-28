import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyAdditionalInfoComponent } from './additional-info.component';

describe('CompanyAdditionalInfoComponent', () => {
  let component: CompanyAdditionalInfoComponent;
  let fixture: ComponentFixture<CompanyAdditionalInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyAdditionalInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyAdditionalInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
