import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanySignatureInfoComponent } from './signature-info.component';

describe('CompanySignatureInfoComponent', () => {
  let component: CompanySignatureInfoComponent;
  let fixture: ComponentFixture<CompanySignatureInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanySignatureInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanySignatureInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
