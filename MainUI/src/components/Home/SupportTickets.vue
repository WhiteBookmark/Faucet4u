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
          <mdb-col col="2"></mdb-col>
          <mdb-col class="text-center">
            <mdb-card border="secondary" class="mb-3">
              <mdb-card-header class="text-secondary"
                >Create new support ticket</mdb-card-header
              >
              <mdb-card-body class="text-secondary">
                <mdb-card-text>
                  <p class="font-weight-bold">Subject</p>
                  <input
                    v-validate="'required|min:10|max:70'"
                    name="subject"
                    v-model="subject"
                    type="text"
                    class="form-control"
                  />

                  <p class="font-weight-bold">Message</p>
                  <textarea
                    v-validate="'required|min:10'"
                    rows="5"
                    name="message"
                    v-model="message"
                    type="text"
                    class="form-control"
                  />

                  <span class="font-weight-bold">
                    {{ errors.first("subject") }} <br />
                    {{ errors.first("message") }} <br />
                  </span>

                  <div class="text-center mt-4">
                    <mdb-btn
                      v-on:click="createSupportTicket()"
                      color="secondary"
                      >Submit</mdb-btn
                    >
                  </div>
                </mdb-card-text>
              </mdb-card-body>
            </mdb-card>
          </mdb-col>
          <mdb-col col="2"></mdb-col>
        </mdb-row>
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
  </div>
</template>

<script>
import {
  mdbRow,
  mdbCol,
  mdbBtn,
  mdbDatatable,
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
    mdbDatatable,
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
      errorList: null,
      successList: null,
      reference: null,
      subject: null,
      message: null
    };
  },
  sockets: {
    createdSupportTicket: function(data) {}
  },
  methods: {
    test: function() {},

    createSupportTicket: function() {
      this.$store.commit("isLoading", true);
      this.$validator.validateAll().then(result => {
        if (result === true) {
          this.$socket.emit(
            "createSupportTicket",
            JSON.stringify({
              sessionId: this.$cookie.get("sessionId"),
              subject: this.subject,
              message: this.message
            })
          );
        }
      });

      this.$store.commit("isLoading", false);
    }
  },
  mounted() {}
};
</script>
