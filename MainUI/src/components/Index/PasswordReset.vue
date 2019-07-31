<template>
  <mdb-row>
    <mdb-col class="text-center">
      <mdb-row>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
        <mdb-col>
          <span class="font-weight-bold">
            {{ errors.first("username") }} <br />
            {{ errors.first("password") }} <br />
            {{ errors.first("confirmPassword") }} <br />
            {{ errors.first("confirmationCode") }} <br />
          </span>

          <p class="font-weight-bold">
            {{ errorList }}
          </p>
          <br />

          <p class="font-weight-bold">
            {{ successList }}
          </p>
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
              <p class="h4 text-center py-4">Password Reset</p>

              <mdb-input
                label="Username"
                group
                icon="user"
                type="text"
                v-validate="'required|min:3|max:10|alpha_num'"
                name="username"
                v-model.trim="username"
              />
              <mdb-input
                label="Your password"
                icon="lock"
                type="password"
                v-validate="'required|min:10|max:128'"
                name="password"
                v-model="password"
                ref="password"
              />
              <mdb-input
                label="Confirm your password"
                icon="exclamation-triangle"
                type="password"
                v-validate="'required|min:10|max:128|confirmed:password'"
                name="confirmPassword"
                v-model="confirmPassword"
              />
              <mdb-input
                label="Confirmation Code"
                icon="code"
                group
                type="text"
                v-validate="
                  'required|regex:(^([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$)'
                "
                name="confirmationCode"
                v-model.trim="confirmationCode"
              />

              <div class="text-center">
                <div
                  id="recaptcha-main"
                  class="g-recaptcha"
                  v-bind:data-sitekey="[recaptchav2SiteKey]"
                  style="display: inline-block;"
                ></div>
                <br />
                <a
                  v-on:click="routerPush('PasswordResetCode')"
                  class="font-weight-bold"
                  >Resend confirmation code ?</a
                >
                <br />
                <mdb-btn
                  outline="secondary"
                  v-on:click="sendPasswordResetApi()"
                  type="button"
                  >Reset <mdb-icon far icon="paper-plane"
                /></mdb-btn>
              </div>
            </mdb-card-body>
          </mdb-card>
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

    <button
      type="button"
      class="bottomLeftButton"
      id="bottomLeftButton"
      onClick="document.getElementById('bottomLeftBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomLeftBanner" id="bottomLeftBanner">
      <square-banner></square-banner>
    </div>

    <button
      type="button"
      class="bottomRightButton"
      id="bottomRightButton"
      onClick="document.getElementById('bottomRightBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomRightBanner" id="bottomRightBanner">
      <square-banner></square-banner>
    </div>
  </mdb-row>
</template>

<script>
import {
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
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      username: null,
      password: null,
      confirmPassword: null,
      confirmationCode: this.$route.query.passwordResetCode,
      apiResponse: null,
      errorList: null,
      successList: null
    };
  },
  created() {
    this.$nextTick(function() {
      grecaptcha.render("recaptcha-main");
    });
  },
  mounted() {},
  methods: {
    test: function() {},
    sendPasswordResetApi: function() {
      //this.$store.commit('isLoading', true);
      this.errorList = this.successList = null;

      this.$validator.validateAll().then(result => {
        if (result === true) {
          grecaptcha
            .execute(this.recaptchav3SiteKey, { action: "Password_Reset" })
            .then(token => {
              this.axios
                .patch(process.env.VUE_APP_API_PasswordReset, {
                  username: this.username,
                  password: this.password,
                  confirmPassword: this.confirmPassword,
                  confirmationCode: this.confirmationCode,
                  recaptchav2Response: grecaptcha.getResponse(),
                  recaptchav3Response: token
                })
                .then(response => {
                  grecaptcha.reset();
                  for (var i in response.data) {
                    this.successList = response.data[i];
                    break;
                  }
                  this.$store.commit("isLoading", false);
                })
                .catch(error => {
                  grecaptcha.reset();
                  for (var i in error.response.data["errors"]) {
                    this.errorList = error.response.data["errors"][i][0];
                    break;
                  }
                  this.$store.commit("isLoading", false);
                });
            });
        }
      });
    },
    routerPush: function(location) {
      this.$router.push(location);
    }
  }
};
</script>
