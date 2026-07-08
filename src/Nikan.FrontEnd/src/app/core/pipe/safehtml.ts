import { Pipe, PipeTransform, Injectable } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Pipe({ standalone: false, name: 'safeHtml' })
@Injectable()
export class SafePipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {}

  transform(html?: string) {
    if (!html) return '';
    return this.sanitizer.bypassSecurityTrustHtml(html);
  }
}
