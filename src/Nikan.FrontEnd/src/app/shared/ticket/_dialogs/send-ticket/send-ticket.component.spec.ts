import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SendTicketDialogComponent } from './send-ticket.component';

describe('SendTicketDialogComponent', () => {
  let component: SendTicketDialogComponent;
  let fixture: ComponentFixture<SendTicketDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SendTicketDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SendTicketDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
