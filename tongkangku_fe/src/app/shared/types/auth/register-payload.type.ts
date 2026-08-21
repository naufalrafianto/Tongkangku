import { User } from '../enum/user.enum';

export type RegisterPayload = {
  name: string;
  email: string;
  password: string;
  role: User;
};
