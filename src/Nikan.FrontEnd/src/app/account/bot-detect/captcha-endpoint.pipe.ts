import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'captchaEndpoint', standalone: false })
export class CaptchaEndpointPipe implements PipeTransform {
  // strip '/' character from the end of the given path.
  transform(value: string): string {
    if (value === undefined || value === null) {
      return value;
    }
    return value.trim().replace(/\/+$/g, '');
  }
}
