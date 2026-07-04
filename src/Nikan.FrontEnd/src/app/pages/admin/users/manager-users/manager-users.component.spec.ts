import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManagerUsersComponent } from './manager-users.component';

describe('AdminManagerUsersComponent', () => {
  let component: AdminManagerUsersComponent;
  let fixture: ComponentFixture<AdminManagerUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminManagerUsersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminManagerUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
