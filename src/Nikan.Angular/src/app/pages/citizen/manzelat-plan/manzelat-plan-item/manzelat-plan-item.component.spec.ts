import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JanbazanComponent } from './janbazan.component';

describe('JanbazanComponent', () => {
  let component: JanbazanComponent;
  let fixture: ComponentFixture<JanbazanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JanbazanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JanbazanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
