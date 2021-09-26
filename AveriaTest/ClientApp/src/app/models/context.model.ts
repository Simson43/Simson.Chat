import { LoginReason } from "./login-reason.model";
import { Message } from "./message.model";
import { User } from "./user.model";

export class Context {
  reason: LoginReason;
  users: User[]
  messages: Message[];
}
