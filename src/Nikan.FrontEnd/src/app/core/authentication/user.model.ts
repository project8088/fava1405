export interface AuthUser {
  userId: string;
  userName: string;
  displayName: string;
  roles: string[] | null;
  rootModule?: string;
  isAdmin?: boolean;
  isCitizen?: boolean;
  isCompany?: boolean;
  isJobseeker?: boolean;
  isCardUser?: boolean;
  isWebUser?: boolean;

  userCompanyStatus?: number;
  rejectDescription?: string;
}
