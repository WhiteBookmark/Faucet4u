<template>
  <mdb-row>
    <mdb-col col="1"></mdb-col>
    <mdb-col class="text-center">
      <mdb-row>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
      </mdb-row>
      <br />

      <p class="h4 text-center mb-4">Support Ticket</p>

      <label class="grey-text">Ticket Reference number</label>
      <input
        v-validate="'required'"
        name="reference"
        v-model.trim="reference"
        type="text"
        class="form-control"
      />
      <br />

      <mdb-row>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
        <mdb-col>
          <div class="text-center mt-4">
            <div
              id="recaptcha-main"
              class="g-recaptcha"
              v-bind:data-sitekey="[recaptchav2SiteKey]"
              style="display: inline-block;"
            ></div>
          </div>
        </mdb-col>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
      </mdb-row>
      <br />

      <mdb-row>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
        <mdb-col>
          <div class="text-center mt-4">
            <mdb-btn
              v-on:click="sendSupportTicketsApi()"
              type="button"
              color="secondary"
              >View</mdb-btn
            >
          </div>
        </mdb-col>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
      </mdb-row>
      <br />

      <mdb-row>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
        <mdb-col>
          <span class="font-weight-bold">
            {{ errors.first("reference") }} <br />
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
      <br />
    </mdb-col>
    <mdb-col col="1"></mdb-col>

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
import { mdbRow, mdbCol, mdbBtn } from "mdbvue";

export default {
  components: { mdbRow, mdbCol, mdbBtn },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      reference: null,
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
    sendSupportTicketsApi: function() {
      this.$store.commit("isLoading", true);
      this.errorList = this.successList = null;

      this.$validator.validateAll().then(result => {
        if (result === true) {
          grecaptcha
            .execute(this.recaptchav3SiteKey, { action: "View_Ticket" })
            .then(token => {
              this.axios
                .get(process.env.VUE_APP_API_SupportTickets, {
                  params: {
                    reference: this.reference,
                    recaptchav2Response: grecaptcha.getResponse(),
                    recaptchav3Response: token
                  }
                })
                .then(response => {
                  grecaptcha.reset();
                  for (var i in response.data) {
                    this.successList = response.data[i];
                    break;
                  }
                  this.$store.commit(
                    "viewTicketsReferenceChange",
                    this.reference
                  );
                  this.$store.commit("isLoading", false);
                  this.$router.push("/SupportTicketsReply");
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
    }
  }
};
</script>
