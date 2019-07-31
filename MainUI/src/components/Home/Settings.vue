<template>
    <div>
        <mdb-row>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col class="text-center">
                <mdb-row>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                </mdb-row>
                <br />
                <br />

                <mdb-row>
                    <mdb-col class="text-center">
                        <mdb-card border="secondary"
                                  class="mb-3 mx-auto"
                                  style="max-width: 18rem;">
                            <mdb-card-header class="text-secondary">Faucethub Bitcoin Address</mdb-card-header>
                            <mdb-card-body class="text-secondary">
                                <mdb-card-text>
                                    <p class="font-weight-bold">Address</p>
                                    <input v-validate="'required'"
                                           name="address"
                                           v-model.trim="address"
                                           type="text"
                                           class="form-control" />

                                    <div class="text-center mt-4">
                                        <mdb-btn v-on:click="sendSettingsPutApi()" color="secondary">Save</mdb-btn>
                                    </div>
                                </mdb-card-text>
                            </mdb-card-body>
                        </mdb-card>
                    </mdb-col>
                    <mdb-col class="text-center">
                        <mdb-card border="secondary"
                                  class="mb-3 mx-auto"
                                  style="max-width: 18rem;">
                            <mdb-card-header class="text-secondary">Change Password</mdb-card-header>
                            <mdb-card-body class="text-secondary">
                                <mdb-card-text>
                                    <p class="font-weight-bold">New Password</p>
                                    <input v-validate="'required|min:10|max:128'"
                                           name="password"
                                           ref="password"
                                           v-model="password"
                                           type="password"
                                           class="form-control" />

                                    <p class="font-weight-bold">Confirm New Password</p>
                                    <input v-validate="'required|min:10|max:128|confirmed:password'"
                                           name="confirmPassword"
                                           v-model="confirmPassword"
                                           type="password"
                                           class="form-control" />

                                    <div class="text-center mt-4">
                                        <mdb-btn v-on:click="changePassword()" color="secondary">Change</mdb-btn>
                                    </div>
                                </mdb-card-text>
                            </mdb-card-body>
                        </mdb-card>
                    </mdb-col>
                </mdb-row>

                <ul class="font-weight-bold">
                    <li v-for="error in errors.all()">{{ error }}</li>
                </ul>

                <p class="font-weight-bold">
                    {{ errorList }}
                </p>
                <br />

                <p class="font-weight-bold">
                    {{ successList }}
                </p>
            </mdb-col>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col col="1"></mdb-col>
        </mdb-row>

        <mdb-row>
            <mdb-col>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </mdb-col>
            <mdb-col>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </mdb-col>
        </mdb-row>

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
    </div>
</template>

<script>
    import
    {
        mdbRow,
        mdbCol,
        mdbBtn,
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
                address: this.$store.state.userData[0].FaucetHubBitcoinAddress,
                apiResponse: null,
                errorList: null,
                successList: null,
                password: null,
                confirmPassword: null
            };
        },
        methods: {
            test: function () { },
            sendSettingsPutApi: function ()
            {
                this.$store.commit("isLoading", true);

                this.errorList = this.successList = null;

                this.$validator.validate("address").then(result =>
                {
                    if (result === true)
                    {
                        grecaptcha
                            .execute(this.recaptchav3SiteKey, {
                                action: "Settings_Update_FaucetHubAddress"
                            })
                            .then(token =>
                            {
                                this.axios
                                    .put(process.env.VUE_APP_API_Settings, {
                                        sessionId: this.$cookie.get("sessionId"),
                                        FaucetHubBitcoinAddress: this.address,
                                        recaptchav3Response: token
                                    })
                                    .then(response =>
                                    {
                                        this.$socket.emit(
                                            "requestingUserData",
                                            JSON.stringify({ sessionId: this.$cookie.get("sessionId") })
                                        );
                                        this.successList.push("Address Updated");
                                    })
                                    .catch(error =>
                                    {
                                        for (var i in error.response.data["errors"])
                                        {
                                            this.errorList = error.response.data["errors"][i][0];
                                            break;
                                        }
                                    });
                            });
                    }
                });

                this.$store.commit("isLoading", false);
            },
            changePassword: function ()
            {
                this.$store.commit("isLoading", true);

                this.errorList = this.successList = null;

                this.$validator.validate("password").then(result =>
                {
                    if (result === true)
                    {
                        this.$validator.validate("confirmPassword").then(result2 =>
                        {
                            if (result2 === true)
                            {
                                grecaptcha
                                    .execute(this.recaptchav3SiteKey, {
                                        action: "Settings_Change_Password"
                                    })
                                    .then(token =>
                                    {
                                        this.axios
                                            .patch(process.env.VUE_APP_API_Settings, {
                                                sessionId: this.$cookie.get("sessionId"),
                                                password: this.password,
                                                recaptchav3Response: token
                                            })
                                            .then(response =>
                                            {
                                                this.$socket.emit(
                                                    "requestingUserData",
                                                    JSON.stringify({
                                                        sessionId: this.$cookie.get("sessionId")
                                                    })
                                                );
                                                this.successList.push("Password Updated");
                                            })
                                            .catch(error =>
                                            {
                                                for (var i in error.response.data["errors"])
                                                {
                                                    this.errorList = error.response.data["errors"][i][0];
                                                    break;
                                                }
                                            });
                                    });
                            }
                        });
                    }
                });

                this.$store.commit("isLoading", false);
            }
        },
        mounted() { }
    };
</script>
