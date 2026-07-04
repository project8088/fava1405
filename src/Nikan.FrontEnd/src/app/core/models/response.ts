 
export class ApiResult<T> {
  isSuccess: boolean;
  statusCode: number;
  messages: string;
  message: string;
  data: T;
}

