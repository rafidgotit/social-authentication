# Introduction

SocialAuthentication .NET library verifies the social login access token in the backend & return necessary infomations to identify the user.

The access token has to be fetched in frontend applicaton by signing in thorugh social authentication provider.

Currently this library supports 3 providers- `google`,`apple` & `facebook`

# Table of Contents

- [Intruduction](#introduction)
- [Installation](#installation)
- [Prerequisities](#prerequisities)
- [Api Reference](#api-reference)
- [Example](#example)
- [Conclusion](#conclusion)

# Installation

Install `Rapplis.SocialAuhentication` from [NuGet Package Manager](https://www.nuget.org/).

```bash
  Install-Package Rapplis.SocialAuhentication
```

# Prerequisities

There is some setup to be done in the frontend part to make this library work smoothly. If you care about only backend, just ignore this section.

- ### Google
  - Setup OAuth client according to [Google Documentation](https://support.google.com/cloud/answer/6158849?hl=en)
  - Make sure to enable [People API](https://developers.google.com/people) in cloud console.
  - For android, make sure your app's SHA-1 is setup in cloud console.
  - While signin in to google, make sure to use the following scopes:<br/>
    `https://www.googleapis.com/auth/userinfo.profile`<br/>
    `https://www.googleapis.com/auth/userinfo.email`<br/>
    `https://www.googleapis.com/auth/user.gender.read`<br/>
    `https://www.googleapis.com/auth/user.birthday.read`
- ### Facebook
  - Create an app in [Meta for Developers](https://developers.facebook.com/) and setup login accroding to the [Documentation](https://developers.facebook.com/docs/facebook-login/)
  - For Android, setup app's hash key in the console.
  - While signing in with facebook, add the following permissions: `email`, `public_profile`, `user_birthday`, `user_gender`.
- ### Apple
  - Follow official [Apple Documentation](https://developer.apple.com/sign-in-with-apple/get-started/) & get `ClientId` for apple sign in.
  - Setup redirect url & deep linking for Android.
  - Make sure to add the following scopes: `email`, `fullName`.
  - You will get user's full name only once when user signs in for the first time. You have to send the name to the backend as Apple is not sending user's full name to backend for safety reasons.

# API Reference

### Methods

- #### **GetUser(SocialAuthData)**

```
  await SocialAuth.GetUser(data);
```

| Parameter Type   | Return Type  |
| :--------------- | :----------- |
| `SocialAuthData` | `SocialUser` |

### Constant Classes

- #### **SocialAuthProvider**
  | Variable | Type     | Value      |
  | :------- | :------- | :--------- |
  | Google   | `string` | `google`   |
  | Facebook | `string` | `facebook` |
  | Apple    | `string` | `apple`    |

### Models

- #### **SocialAuthData**

  This model is accepted as peremeter in `GetUser` method.
  | Field | Type | Description |
  | :------------ | :------- | :--------------------------------------------------------------------- |
  | Provider | `string` | **required\*** provider name mentioned in `SocialAuthProvider` |
  | Token | `string` | **required\*** access token fetched through signing in social provider |
  | AppleClientId | `string` | **required for apple\*** client id from apple developer console |

- #### **SocialUser**
  This model is returned by `GetUser` method.
  | Field | Type | Description |
  | :------------ | :---------- | :------------------------------------------------------------------------------- |
  | Id | `string` | Unique ID fetched from login provider which has to be used for identifying users |
  | Email | `string?` | `nullable` Email address of the user |
  | EmailVerified | `bool` | If email address have verified by social auth provider |
  | Name | `String?` | `nullable` User's full name |
  | FirstName | `String?` | `nullable` First Name of the user |
  | LastName | `String?` | `nullable` Last Name of the user |
  | Picture | `String?` | `nullable` Image url of user's profile picture |
  | Locale | `String?` | `nullable` User's locale |
  | Gender | `String?` | `nullable` Gender of the user |
  | Birthday | `DateTime?` | `nullable` User's birthday |
  | Provider | `String` | provider name mentioned in `SocialAuthProvider` |

# Example

First you have to get the access token of a provider from user through api or however you are implementing. Pass the provider name to validate the access token and get a user with unique id provided by the social authentication provider. Here is an example of Google Authentication.

```
var googleAuthData = new SocialAuthData {
  Token = "access_token_got_from_user",
  Provider = SocialAuthProvider.Google,
};
SocialUser? socialUser = null;
try
{
  socialUser = await SocialAuth.GetUser(googleAuthData);
}
catch (Exception e)
{
  //todo handle error
}
```

# Conclusion

The objective of this library is to verify social authentication token and get user data froms social authentication providers in the backend.
Just as beginning, it supports only 3 providers- `google`, `apple` & `facebook`. Hope to implement more providers soon. Even though it is properly tested, it may have some edge case issues. Feel free to submit issue on [Github Repository](#https://github.com/rafid08/social-authentication). You're also welcome to contribute to this package.

Follow me for more on [github/rafid08](#https://github.com/rafid08)

<a href="https://www.buymeacoffee.com/rafid" target="_blank"><img style="border-radius:7px" src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>
