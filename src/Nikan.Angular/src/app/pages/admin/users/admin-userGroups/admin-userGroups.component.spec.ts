import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminUserGroupsComponent } from './admin-userGroups.component';

 

describe('AdminUserGrouplistComponent', () => {
  let component: AdminUserGroupsComponent;
  let fixture: ComponentFixture<AdminUserGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminUserGroupsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
