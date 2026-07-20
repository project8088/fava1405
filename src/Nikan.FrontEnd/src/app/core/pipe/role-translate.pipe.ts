import { Pipe, PipeTransform } from '@angular/core';
import { ROLES } from '@core/authentication/auth.service';

@Pipe({
  standalone: false,
  name: 'roleTranslate',
})
export class RoleTranslatePipe implements PipeTransform {
  transform(role?: string, showBase = true) {
    if (!role) return '';
    let title = '';
    switch (role.toLowerCase()) {
      case ROLES.admin:
        title = 'ادمین';
        break;
      case ROLES.user:
        title = 'کاربر';
        break;
      case ROLES.company:
        title = 'شرکت';
        break;
      case ROLES.citizen:
        title = 'شهروندی';
        break;
      case ROLES.card:
        title = 'مدیریت کارت';
        break;
      case ROLES.webapiuser:
        title = 'توسعه دهنده';
        break;
    }
    if (showBase) {
      title += ' ' + role;
    }
    return title;
  }
}
