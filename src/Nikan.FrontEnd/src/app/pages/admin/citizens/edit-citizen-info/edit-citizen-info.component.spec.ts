import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminEditCitizenInfoComponent } from './edit-citizen-info.component';

describe('AdminEditCitizenInfoComponent', () => {
  let component: AdminEditCitizenInfoComponent;
  let fixture: ComponentFixture<AdminEditCitizenInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminEditCitizenInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminEditCitizenInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
