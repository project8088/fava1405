import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobseekerProfileDialogComponent } from './jobseeker-profile.component';

describe('JobseekerProfileDialogComponent', () => {
  let component: JobseekerProfileDialogComponent;
  let fixture: ComponentFixture<JobseekerProfileDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [JobseekerProfileDialogComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobseekerProfileDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
