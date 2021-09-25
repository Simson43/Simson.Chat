import { Message } from "./message.model";
import { User } from "./user.model";

export class Context {
  Users: User[]
  Messages: Message[];
}
