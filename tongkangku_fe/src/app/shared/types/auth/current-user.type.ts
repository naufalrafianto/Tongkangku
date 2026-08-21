import { ApiResponse } from '../api/response.type';

export interface CurrentUserResponse {
  id: string;
  name: string;
  email: string;
  role: number;
  createdAt: string;
  updatedAt: string;
}

export type CurrentUserApiResponse = ApiResponse<CurrentUserResponse>;
