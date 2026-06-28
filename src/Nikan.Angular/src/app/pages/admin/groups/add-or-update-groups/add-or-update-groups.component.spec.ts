import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAddOrUpdateAppserviceComponent } from './add-or-update-appservice.component';

describe('AddOrUpdateAppserviceComponent', () => {
  let component: AdminAddOrUpdateAppserviceComponent;
  let fixture: ComponentFixture<AddOrUpdateAppserviceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminAddOrUpdateAppserviceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAddOrUpdateAppserviceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
