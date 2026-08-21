import { ApiResponse } from '../api/response.type';

export interface RegisterResponse {
  name: string;
  email: string;
  role: string;
  id: string;
}

export type RegisterApiResponse = ApiResponse<RegisterResponse>;
