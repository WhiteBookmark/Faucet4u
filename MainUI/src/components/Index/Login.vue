<template>
    <mdb-row>
        <mdb-col class="text-center">
            <mdb-row>
                <mdb-col>
                    <standard-banner></standard-banner>
                </mdb-col>
                <mdb-col>
                    <span class="font-weight-bold">
                        {{ errors.first("username") }}<br />
                        {{ errors.first("password") }}
                    </span>
                    <br />

                    <p class="font-weight-bold">
                        {{ errorList }}
                    </p>
                    <br />
                </mdb-col>
                <mdb-col>
                    <standard-banner></standard-banner>
                </mdb-col>
            </mdb-row>

            <mdb-row>
                <mdb-col>
                    <mdb-row>
                        <mdb-col>
                            <skyscrapper-banner></skyscrapper-banner>
                        </mdb-col>
                        <mdb-col>
                            <skyscrapper-banner></skyscrapper-banner>
                        </mdb-col>
                    </mdb-row>
                </mdb-col>
                <mdb-col>
                    <mdb-card>
                        <mdb-card-body class="text-left">
                            <p class="h4 text-center py-4">Login</p>

                            <mdb-input label="Username"
                                       group
                                       icon="user"
                                       type="text"
                                       v-validate="'required|min:3|max:10|alpha_num'"
                                       name="username"
                                       v-model.trim="username" />
                            <mdb-input label="Your password"
                                       icon="lock"
                                       type="password"
                                       v-validate="'required|min:10|max:128'"
                                       name="password"
                                       v-model="password" />

                            <div class="text-center">
                                <div id="recaptcha-main"
                                     class="g-recaptcha"
                                     v-bind:data-sitekey="[recaptchav2SiteKey]"
                                     style="display: inline-block;"></div>
                                <br />
                                <a v-on:click="resetPassword()" class="font-weight-bold">Forgot password ?</a>
                                <br />
                                <mdb-btn outline="secondary"
                                         v-on:click="sendLoginApi()"
                                         type="button">
                                    Login <mdb-icon far icon="paper-plane" />
                                </mdb-btn>
                            </div>
                        </mdb-card-body>
                    </mdb-card>
                    <br />
                    <standard-banner></standard-banner>
                    <br />
                    <standard-banner></standard-banner>
                </mdb-col>
                <mdb-col>
                    <mdb-row>
                        <mdb-col>
                            <skyscrapper-banner></skyscrapper-banner>
                        </mdb-col>
                        <mdb-col>
                            <skyscrapper-banner></skyscrapper-banner>
                        </mdb-col>
                    </mdb-row>
                </mdb-col>
            </mdb-row>
        </mdb-col>

        <button type="button"
                class="bottomLeftButton"
                id="bottomLeftButton"
                onClick="document.getElementById('bottomLeftBanner').remove();this.remove();">
            x
        </button>
        <div class="bottomLeftBanner" id="bottomLeftBanner">
            <square-banner></square-banner>
        </div>

        <button type="button"
                class="bottomRightButton"
                id="bottomRightButton"
                onClick="document.getElementById('bottomRightBanner').remove();this.remove();">
            x
        </button>
        <div class="bottomRightBanner" id="bottomRightBanner">
            <square-banner></square-banner>
        </div>
    </mdb-row>
</template>

<script>
    import
    {
        mdbRow,
        mdbCol,
        mdbBtn,
        mdbIcon,
        mdbInput,
        mdbCard,
        mdbCardHeader,
        mdbCardBody,
        mdbCardTitle,
        mdbCardText
    } from "mdbvue";

    export default {
        components: {
            mdbRow,
            mdbCol,
            mdbBtn,
            mdbIcon,
            mdbInput,
            mdbCard,
            mdbCardHeader,
            mdbCardBody,
            mdbCardTitle,
            mdbCardText
        },
        data()
        {
            return {
                recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
                recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
                username: null,
                password: null,
                tokenx: null,
                apiResponse: null,
                sessionValid: this.sessionValid,
                errorList: null,
                ip: "0.0.0.0"
            };
        },
        mounted()
        {
            this.$nextTick(function ()
            {
                grecaptcha.render("recaptcha-main");
            });

            this.axios
                .get(process.env.VUE_APP_API_IP)
                .then(response =>
                {
                    this.ip = response.data.ip;
                })
                .catch(error => { });
        },
        methods: {
            test: function () { },
            sendLoginApi: function ()
            {
                this.$store.commit("isLoading", true);
                this.errorList = this.successList = null;
                this.$validator.validateAll().then(result =>
                {
                    if (result === true)
                    {
                        grecaptcha
                            .execute(this.recaptchav3SiteKey, { action: "Login" })
                            .then(token =>
                            {
                                this.axios
                                    .post(process.env.VUE_APP_API_Login, {
                                        username: this.username,
                                        password: this.password,
                                        recaptchav2Response: grecaptcha.getResponse(),
                                        recaptchav3Response: token,
                                        lsi: this.$cookie.get("lsi"),
                                        ip: this.ip
                                    })
                                    .then(response =>
                                    {
                                        grecaptcha.reset();
                                        this.apiResponse = response.data;
                                        this.$cookie.set("sessionId", this.apiResponse.sessionId, {
                                            expires: "6h"
                                        });
                                        this.$socket.emit(
                                            "requestingUserData",
                                            JSON.stringify({ sessionId: this.$cookie.get("sessionId") })
                                        );
                                        this.$store.commit("isLoading", false);
                                    })
                                    .catch(error =>
                                    {
                                        grecaptcha.reset();
                                        for (var i in error.response.data["errors"])
                                        {
                                            this.errorList = error.response.data["errors"][i][0];
                                            break;
                                        }
                                        this.$store.commit("isLoading", false);
                                    });
                            });
                    }
                });
            },
            resetPassword()
            {
                this.$router.push("PasswordResetCode");
            }
        }
    };
</script>
