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
        <mdb-col col="3">
          <skyscrapper-banner></skyscrapper-banner>
        </mdb-col>
        <mdb-col class="text-center">
          <mdb-card border="secondary" class="mb-3">
            <mdb-card-header class="text-secondary">
              <span v-if="status">Status: Closed</span>
              <span v-else>Status: Open</span>
            </mdb-card-header>
            <mdb-card-body class="text-secondary">
              <mdb-card-title tag="h5">Subject: {{ subject }}</mdb-card-title>
              <mdb-card-text>
                {{ message }}
              </mdb-card-text>
            </mdb-card-body>
            <mdb-card-footer border="secondary" tag="h5" class="text-secondary"
              >Created: {{ this.moment(dateTime) }}</mdb-card-footer
            >
          </mdb-card>

          <div v-for="value in replyList">
            <div v-if="value.Username === 'Admin'">
              <mdb-card border="primary" class="mb-3">
                <mdb-card-header class="text-primary">Admin</mdb-card-header>
                <mdb-card-body class="text-primary">
                  <mdb-card-text>
                    {{ value.Reply }}
                  </mdb-card-text>
                </mdb-card-body>
                <mdb-card-footer
                  border="primary"
                  tag="h5"
                  class="text-primary"
                  >{{ this.moment(value.DateTime) }}</mdb-card-footer
                >
              </mdb-card>
            </div>
            <div v-else>
              <mdb-card border="secondary" class="mb-3">
                <mdb-card-header class="text-secondary">{{
                  value.Username
                }}</mdb-card-header>
                <mdb-card-body class="text-secondary">
                  <mdb-card-text>
                    {{ value.Reply }}
                  </mdb-card-text>
                </mdb-card-body>
                <mdb-card-footer
                  border="secondary"
                  tag="h5"
                  class="text-secondary"
                  >{{ this.moment(value.DateTime) }}</mdb-card-footer
                >
              </mdb-card>
            </div>
          </div>

          <mdb-card border="secondary" class="mb-3">
            <mdb-card-header class="text-secondary">Reply form</mdb-card-header>
            <mdb-card-body class="text-secondary">
              <mdb-card-text>
                <p class="font-weight-bold">Message</p>
                <textarea
                  v-validate="'required|min:10'"
                  name="reply"
                  rows="6"
                  v-bind:disabled="status"
                  v-model="reply"
                  type="text"
                  class="form-control"
                />

                <span class="font-weight-bold">
                  {{ errors.first("reply") }}
                </span>

                <p class="font-weight-bold">
                  {{ errorList }}
                </p>
                <br />

                <p class="font-weight-bold">
                  {{ successList }}
                </p>

                <div
                  id="recaptcha-main"
                  class="g-recaptcha"
                  v-bind:data-sitekey="[recaptchav2SiteKey]"
                  style="display: inline-block;"
                ></div>

                <div class="text-center mt-4">
                  <mdb-btn
                    v-on:click="sendSupportTicketsReplyApi()"
                    color="secondary"
                    v-bind:disabled="status"
                    >Reply</mdb-btn
                  >
                </div>
              </mdb-card-text>
            </mdb-card-body>
          </mdb-card>
        </mdb-col>
        <mdb-col col="3">
          <skyscrapper-banner></skyscrapper-banner>
        </mdb-col>
      </mdb-row>
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
import {
  mdbRow,
  mdbCol,
  mdbBtn,
  mdbCard,
  mdbCardHeader,
  mdbCardBody,
  mdbCardTitle,
  mdbCardText,
  mdbCardFooter
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
    mdbCardText,
    mdbCardFooter
  },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      subject: null,
      message: null,
      status: null,
      dateTime: null,
      apiResponse: null,
      reply: null,
      errorList: null,
      successList: null,
      replyList: []
    };
  },
  created() {
    this.$nextTick(function() {
      grecaptcha.render("recaptcha-main");
    });
  },
  methods: {
    test: function() {},
    sendSupportTicketsReplyApi: function() {
      this.$store.commit("isLoading", true);
      this.errorList = this.successList = null;

      this.$validator.validateAll().then(result => {
        if (result === true) {
          grecaptcha
            .execute(this.recaptchav3SiteKey, {
              action: "Support_Tickets_Reply"
            })
            .then(token => {
              this.axios
                .post(
                  process.env.VUE_APP_API_SupportTicketsReply,

                  {
                    reference: this.$store.state.viewTicketsReference,
                    reply: this.reply,
                    recaptchav2Response: grecaptcha.getResponse(),
                    recaptchav3Response: token
                  }
                )
                .then(response => {
                  grecaptcha.reset();
                  for (var i in response.data) {
                    this.successList = response.data[i];
                    break;
                  }
                  this.$store.commit("isLoading", false);
                  this.requestSupportTicketsReply();
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
    requestSupportTicket: function() {
      this.$store.commit("isLoading", true);
      grecaptcha
        .execute(this.recaptchav3SiteKey, {
          action: "Requested_Support_Ticket"
        })
        .then(token => {
          this.axios
            .get(process.env.VUE_APP_API_SupportTickets, {
              params: {
                reference: this.$store.state.viewTicketsReference,
                recaptchav3Response: token
              }
            })
            .then(response => {
              this.apiResponse = response.data;
              this.status = this.apiResponse["Locked"];
              this.subject = this.apiResponse["Subject"];
              this.message = this.apiResponse["Message"];
              this.dateTime = this.apiResponse["DateTime"];
              this.$store.commit("isLoading", false);
            })
            .catch(error => {
              for (var i in error.response.data["errors"]) {
                this.errorList = error.response.data["errors"][i][0];
                break;
              }
              this.$store.commit("isLoading", false);
            });
        });
      this.$store.commit("isLoading", false);
    },
    requestSupportTicketsReply: function() {
      this.replyList = [];

      this.$store.commit("isLoading", true);
      grecaptcha
        .execute(this.recaptchav3SiteKey, { action: "Requested_All_Replies" })
        .then(token => {
          this.axios
            .get(process.env.VUE_APP_API_SupportTicketsReply, {
              params: {
                reference: this.$store.state.viewTicketsReference,
                recaptchav3Response: token
              }
            })
            .then(response => {
              this.apiResponse = response.data;

              for (var i = 0; i < Object.keys(this.apiResponse).length; i++) {
                this.replyList.push(this.apiResponse[i]);
              }
              this.$store.commit("isLoading", false);
            })
            .catch(error => {
              for (var i in error.response.data["errors"]) {
                this.errorList = error.response.data["errors"][i][0];
                break;
              }
              this.$store.commit("isLoading", false);
            });
        });
      this.$store.commit("isLoading", false);
    }
  },
  mounted() {
    //Because we have to use 2 methods simultaneously, first set loader true here, and only set loader false in the end of each method
    this.$store.commit("isLoading", true);
    this.requestSupportTicket();
    this.requestSupportTicketsReply();
    this.$store.commit("isLoading", false);
  }
};
</script>
