using EmailValidation;
using System;

namespace Psico
{
    class CheckExistMail
    {
        public string CheckMail(string mail)
        {
            // Обновление переменной
            string MailStatus = "";

            try
            {
                // Проверка указанной почты на существование
                EmailValidator emailValidator = new EmailValidator();
                EmailValidationResult result;

                // Если результата проверки нет
                if (!emailValidator.Validate(mail, out result))
                {
                    // Вывод сообщения
                    MailStatus = "Ошибка при проверке указанной почты на существование, если указаная почта корректна проверьте подключение к интернету или обратитесь к администратору!";
                }
                else
                {
                    // Запись в переменную данных, в зависимости от результата проверки почты
                    switch (result)
                    {
                        case EmailValidationResult.OK:
                            MailStatus = "Почтовый ящик существует";
                            break;

                        case EmailValidationResult.MailboxUnavailable:
                            MailStatus = "Указаная почта не существует";
                            break;

                        case EmailValidationResult.MailboxStorageExceeded:
                            MailStatus = "Указанный почтовый ящик переполнен";
                            break;

                        case EmailValidationResult.NoMailForDomain:
                            MailStatus = "Письма для такого домена не настроены";
                            break;
                        default:
                            MailStatus = "Ошибка при проверке указанной почты на существование, если указаная почта корректна обратитесь к администратору!";
                            break;
                    }
                }
            }
            catch
            {
                MailStatus = "Ошибка при проверке указанной почты на существование, если указаная почта корректна проверьте подключение к интернету или обратитесь к администратору!";
            }

            // Возвращение результата проверки почты на существование
            return Convert.ToString(MailStatus);
        }
    }
}
