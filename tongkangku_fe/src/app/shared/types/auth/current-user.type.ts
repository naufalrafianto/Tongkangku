import { ApiResponse } from '../api/response.type';

export interface CurrentUser {
  id: string;
  name: string;
  email: string;
  role: number;
  createdAt: string;
  updatedAt: string;
}

export type CurrentUserResponse = ApiResponse<CurrentUser>;
