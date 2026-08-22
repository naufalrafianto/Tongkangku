import { ApiResponse } from '../api/response.type';

export interface LoginResponse {
  token: string;
}

export type LoginApiResponse = ApiResponse<LoginResponse>;
