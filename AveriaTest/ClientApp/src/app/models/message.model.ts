import { User } from "./user.model";

export class Message {
  date?: Date;
  text: string
  user: User;
}
