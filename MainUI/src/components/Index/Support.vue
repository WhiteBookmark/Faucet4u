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

      <mdb-row>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
        <mdb-col>
          <div class="text-center mt-4">
            <mdb-btn
              v-on:click="createSupportTicketsSection()"
              type="button"
              color="secondary"
              >Open New Support Ticket</mdb-btn
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
          <div class="text-center mt-4">
            <mdb-btn
              v-on:click="viewSupportTicketsSection()"
              type="button"
              color="secondary"
              >View Support Ticket</mdb-btn
            >
          </div>
        </mdb-col>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
      </mdb-row>

      <mdb-row>
        <mdb-col>
          <standard-banner></standard-banner>
        </mdb-col>
        <mdb-col>
          <ul class="font-weight-bold">
            <li v-for="value in errorList">
              {{ value }}
            </li>
          </ul>
          <br />

          <ul class="font-weight-bold">
            <li v-for="value in successList">
              {{ value }}
            </li>
          </ul>
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
      message: null,
      subject: null,
      email: null,
      apiResponse: null,
      errorList: null,
      successList: null
    };
  },
  mounted() {},
  methods: {
    test: function() {},
    createSupportTicketsSection: function() {
      this.$router.push("SupportTickets");
    },
    viewSupportTicketsSection: function() {
      this.$router.push("ViewTickets");
    }
  }
};
</script>
